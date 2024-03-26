// authService.js
import { FastifyInstance, FastifyPluginAsync, FastifyReply, FastifyRequest } from 'fastify';
import fp from 'fastify-plugin';
import TokenRequest from './TokenRequest';

declare module 'fastify' {
  interface FastifyInstance {
    authenticate: (clientId: string, clientSecret: string) => Promise<string>;
    verifyJWT: (request: FastifyRequest, reply: FastifyReply) => Promise<void>;
  }
}


const authPlugin: FastifyPluginAsync = async (fastify: FastifyInstance) => {

  // Add a method to verify a JWT token to be used as a preHandler in routes
  fastify.decorate('verifyJWT', async (request: FastifyRequest, reply: FastifyReply) => {
    try {
      await request.jwtVerify();
    } catch (err) {
      reply.status(401).send({ error: 'Authentication failed' });
      throw new Error('Authentication failed');
    }
  });

  // Autenticate a client and return a JWT token
  fastify.decorate('authenticate', async (clientId: string, clientSecret: string) => {
    const isValid = await validateClientCredentials(clientId, clientSecret);
    if (!isValid) throw new Error('Invalid client credentials');

    return fastify.jwt.sign({ client_id: clientId }, { expiresIn: '365d' });
  });

  // Well-known endpoint
  fastify.get('/.well-known/openid-configuration', async (request, reply) => {
    reply.send({
      issuer: process.env.HOST,
      token_endpoint: `${process.env.HOST}/token`,
      token_endpoint_auth_methods_supported: ["client_secret_post"],
    });
  });

  const tokenSchema = {
    description: 'post some data',
  // Schema for the request body
  body: {
    type: 'object',
    required: ['client_id', 'client_secret', 'grant_type'],
    properties: {
      client_id: { type: 'string' },
      client_secret: { type: 'string' },
      grant_type: { type: 'string', enum: ['client_credentials'] }
    }
  },
  // Schema for the successful response
  response: {
    200: {
      type: 'object',
      properties: {
        access_token: { type: 'string' },
        token_type: { type: 'string', default: 'Bearer' },
        expires_in: { type: 'number' }
      }
    },
    // You can define additional responses for different status codes if needed
    400: {
      type: 'object',
      properties: {
        error: { type: 'string' }
      }
    },
    401: {
      type: 'object',
      properties: {
        error: { type: 'string' }
      }
    }
  }
  };

  // Route to get a JWT token
  fastify.post('/token', { schema: tokenSchema }, async (request, reply) => {
    try {
      const { client_id, client_secret, grant_type } = request.body as TokenRequest;

      // Only client_credentials grant type is supported for now/for ever
      if (grant_type !== 'client_credentials') {
        reply.status(400).send({ error: 'Invalid grant type' });
        return;
      }

      const token = await fastify.authenticate(client_id, client_secret);

      reply.send({ access_token: token, token_type: 'Bearer', expires_in: 3600 });
    } catch (error) {
      if (error instanceof Error) {
        reply.status(401).send({ error: error.message });
      } else {
        reply.status(500).send({ error: 'An unknown error occurred' });
      }
    }
  });

  // Hjelpefunksjon for å validere klient-credentials
  async function validateClientCredentials(clientId: string, clientSecret: string) {
    return clientId === process.env.CLIENT_ID && clientSecret === process.env.CLIENT_SECRET;
  }
};

export default fp(authPlugin, { name: 'authPlugin' });