import { headers } from 'next/headers';
import createTranslation from 'next-translate/createTranslation';

import { Container, Layout } from '@/components/ui';
import Heading from '@/components/ui/Heading';
import Section from '@/components/ui/Section';
import { apiWrapper, createSDK } from '@/utils/api/EventurasApi';
import Environment from '@/utils/Environment';
import Logger from '@/utils/Logger';

import UserEditor from '../UserEditor';

type EventInfoProps = {
  params: {
    id: string;
  };
};
const AdminUserDetailPage: React.FC<EventInfoProps> = async ({ params }) => {
  const { t } = createTranslation();

  const eventuras = createSDK({
    baseUrl: Environment.NEXT_PUBLIC_BACKEND_URL,
    authHeader: headers().get('Authorization'),
  });

  const user = await apiWrapper(() =>
    eventuras.users.getV3Users({
      id: params.id,
    })
  );

  if (!user.ok) {
    Logger.error(
      { namespace: 'EditEventinfo' },
      `Failed to fetch order id ${params.id}, error: ${user.error}`
    );
  }

  if (!user.ok) {
    return <div>{t('admin:users.labels.userNotFound')}</div>;
  }

  return (
    <Layout fluid>
      <Section className="bg-white dark:bg-black pb-8">
        <Container>
          <Heading as="h1">{t('admin:users.detailspage.title')}</Heading>
        </Container>
      </Section>
      <Section className="py-12">
        <Container>
          <UserEditor user={user.value!} adminMode />
        </Container>
      </Section>
    </Layout>
  );
};

export default AdminUserDetailPage;