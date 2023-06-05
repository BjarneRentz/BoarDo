<script>
	import { page } from '$app/stores';
	import { onMount } from 'svelte';
	onMount(async () => {
		console.log($page.data.session);
	});
</script>

<p>
	{#if $page.data.session}
		{#if $page.data.session.user?.image}
			<span style="background-image: url('{$page.data.session.user.image}')" class="avatar" />
		{/if}
		<span>
			<small>Signed in as</small><br />
			<strong>{$page.data.session.user?.name ?? 'User'}</strong>
		</span>
	{/if}
</p>
