import {
  EventDto,
  EventDtoPageResponseDto,
  EventFormDto,
  Eventuras,
  NewProductDto,
  NewRegistrationDto,
  ProductDto,
  RegistrationDto,
} from '@losol/eventuras';

import Logger from '@/utils/Logger';

import apiFetch from '../apiFetch';
import ApiResult from '../ApiResult';
import ApiURLs from '../ApiUrls';

const eventuras = new Eventuras();
export type GetEventsOptions = Parameters<typeof eventuras.events.getV3Events>[0];
export type GetEventRegistrationsOptions = Parameters<
  typeof eventuras.registrations.getV3Registrations
>[0];

export const createEventRegistration = async (
  newRegistration: NewRegistrationDto,
  selectedProducts?: Map<string, number>
) => {
  const products = selectedProducts
    ? Array.from(selectedProducts, ([productId, quantity]) => ({
        productId,
        quantity,
      }))
    : [];

  const registration = apiFetch(ApiURLs.registrations(), {
    method: 'POST',
    body: JSON.stringify(newRegistration),
  });
  Logger.info({ namespace: 'events:createEventRegistration' }, 'products selected', products);

  if (!products.length) return registration;

  return registration.then(async (apiResult: ApiResult<RegistrationDto>) => {
    if (!apiResult.ok) {
      return apiResult;
    }
    const result: RegistrationDto = apiResult.value;
    const registrationId = result.registrationId!.toString();
    return await apiFetch(ApiURLs.products({ registrationId }), {
      method: 'POST',
      body: JSON.stringify({
        lines: products,
      }),
    });
  });
};
export const createEvent = (
  formValues: EventFormDto,
  init?: RequestInit | undefined
): Promise<ApiResult<EventDto>> =>
  apiFetch(ApiURLs.events(), { method: 'POST', body: JSON.stringify(formValues), ...init });
export const createProduct = (
  eventId: string,
  product: NewProductDto,
  init?: RequestInit | undefined
): Promise<ApiResult<EventDto>> =>
  apiFetch(ApiURLs.eventProducts({ eventId }), {
    method: 'POST',
    body: JSON.stringify(product),
    ...init,
  });
export const updateEvent = (
  eventId: string,
  formValues: EventFormDto
): Promise<ApiResult<EventDto>> =>
  apiFetch(ApiURLs.event({ eventId }), { method: 'PUT', body: JSON.stringify(formValues) });
export const getEvents = (
  options: GetEventsOptions = {}
): Promise<ApiResult<EventDtoPageResponseDto>> => apiFetch(ApiURLs.events(options));
export const getEvent = (eventId: string): Promise<ApiResult<EventDto>> =>
  apiFetch(ApiURLs.event({ eventId }));
export const getEventProducts = (eventId: string): Promise<ApiResult<ProductDto[]>> =>
  apiFetch(ApiURLs.eventProducts({ eventId }));
export const getEventRegistrations = async (options: GetEventRegistrationsOptions) =>
  apiFetch(ApiURLs.eventRegistrations(options));
