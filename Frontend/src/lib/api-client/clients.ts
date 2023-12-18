import { Apity } from '@cocreators-ee/apity';
import type { paths } from '$lib/api-client/boardo';
import { PUBLIC_BASE_URL } from '$env/static/public';

const apity = Apity.for<paths>();
apity.configure({
	baseUrl: PUBLIC_BASE_URL
});

export const getAuthClients = apity.path('/api/Auth').method('get').create();

export const getConnectProviderUrl = apity
	.path('/api/Auth/Connect/{provider}')
	.method('get')
	.create();

export const deleteProvider = apity.path('/api/Auth/{provider}').method('delete').create();

export const getCalendarSyncState = apity.path('/api/Calendar/SyncState').method('get').create();

export const postToogleCalendarSync = apity
	.path('/api/Calendar/Sync/{enable}')
	.method('post')
	.create();
