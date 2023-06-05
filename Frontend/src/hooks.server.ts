import { SvelteKitAuth } from '@auth/sveltekit';
import Google from '@auth/core/providers/google';
import { AUTH_SECRET, GOOGLE_ID, GOOGLE_SECRET } from '$env/static/private';
import type { RequestEvent } from './routes/$types';
import { redirect, type MaybePromise, type Handle, type ResolveOptions } from '@sveltejs/kit';
import { sequence } from '@sveltejs/kit/hooks';

async function authorize({ event, resolve }) {
	// Protect routes
	if (!event.url.pathname.startsWith('/login')) {
		const session = await event.locals.getSession();
		if (!session) {
			throw redirect(303, '/login');
		}
	}

	// If the request is still here, just proceed as normally
	return resolve(event);
}

export const handle = sequence(
	SvelteKitAuth({
		providers: [Google({ clientId: GOOGLE_ID, clientSecret: GOOGLE_SECRET })],

		callbacks: {
			async jwt({ token, account }) {
				if (account) {
					token = Object.assign({}, token, { jwt: account.id_token });
				}
				return token;
			},
			async session({ session, token }) {
				if (session) {
					session = Object.assign({}, session, { jwt: token.jwt });
					console.log(session);
				}
				return session;
			}
		}
	}),
	authorize
);
