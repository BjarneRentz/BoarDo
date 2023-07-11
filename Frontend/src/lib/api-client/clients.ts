import { AuthApi } from './apis';
import { Configuration } from './runtime';

const config = new Configuration({ basePath: '' });

const authApi = new AuthApi(config);

export { authApi };
