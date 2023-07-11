import { AuthApi, CalendarApi } from './apis';
import { Configuration } from './runtime';

const config = new Configuration({ basePath: '' });

const authApi = new AuthApi(config);

const calendarApi = new CalendarApi(config);

export { authApi, calendarApi };
