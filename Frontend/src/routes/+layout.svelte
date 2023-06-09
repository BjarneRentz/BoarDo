<script>
	import '../app.css';
	import { page } from '$app/stores';
	import { signOut } from '@auth/sveltekit/client';
</script>

{#if $page.data.session}
	<div class="navbar bg-base-100">
		<div class="flex-1">
			<a class="btn btn-ghost normal-case text-xl" href="/">BoarDo</a>
		</div>
		<div class="flex-none gap-2">
			<div class="dropdown dropdown-end">
				<label tabindex="0" class="btn btn-ghost btn-circle avatar">
					<div class="w-10 rounded-full">
						<img src={$page.data.session?.user?.image} />
					</div>
				</label>
				<ul
					tabindex="0"
					class="mt-3 p-2 shadow menu menu-sm dropdown-content bg-base-100 rounded-box w-52"
				>
					<li><a>Settings</a></li>
					<li><a on:click={() => signOut()}>Logout</a></li>
				</ul>
			</div>
		</div>
	</div>
{/if}

<slot />
