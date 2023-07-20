import { Apity } from '@cocreators-ee/apity';
import type { paths } from '$lib/api-client/boardo';

const apity = Apity.for<paths>();
apity.configure({
	baseUrl: 'http://localhost:5117'
});

export const getAuthClients = apity.path('/api/Auth').method('get').create();

export const getConnectGoogleUrl = apity.path('/api/Auth/Connect/Google').method('get').create();

export const getCalendarSyncState = apity.path('/api/Calendar/SyncState').method('get').create();

export const postToogleCalendarSync = apity
	.path('/api/Calendar/Sync/{enable}')
	.method('post')
	.create();
