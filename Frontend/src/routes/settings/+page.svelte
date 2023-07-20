<script lang="ts">
	import { browser } from '$app/environment';
	import {
		getAuthClients,
		getCalendarSyncState,
		getConnectGoogleUrl,
		postToogleCalendarSync
	} from '$lib/api-client/clients';

	const getProvidersRequest = getAuthClients({});
	const getProvidersReady = getProvidersRequest.ready;

	let getCalendarSyncStateReady = getCalendarSyncState({}).ready;

	const connectProvider = async (name: string) => {
		if (name === 'Google' && browser) {
			const result = await getConnectGoogleUrl({}).result;
			window.location.href = result.data.url;
		}
	};

	const toggleCalendarSync = async (enable: boolean) => {
		await postToogleCalendarSync({ enable }).result;
		getCalendarSyncStateReady = getCalendarSyncState({}).ready;
	};
</script>

<div class="flex flex-col gap-y-5">
	<p class="text-3xl font-bold">Einstellungen</p>

	<div class="flex flex-col gap-y-2">
		<p class="text-xl font-bold">Accounts</p>

		<div class="grid grid-cols-2 gap-8">
			{#await $getProvidersReady}
				<progress class="progress progress-primary col-span-2" />
			{:then result}
				{#if result?.ok}
					{#each Object.entries(result.data) as [key, value]}
						<p class="text-lg">{key}</p>
						{#if value}
							<button class="btn btn-error">Löschen</button>
						{:else}
							<button on:click={() => connectProvider(key)} class="btn btn-primary"
								>Verbinden</button
							>
						{/if}
					{/each}
				{:else}
					<p>Providers konnten nicht geladen werden</p>
				{/if}
			{/await}
		</div>
	</div>

	<div class="flex flex-col gap-y-2">
		<p class="text-xl font-bold">Sync</p>

		<div class="grid grid-cols-2 gap-8">
			{#await $getCalendarSyncStateReady}
				<progress class="progress progress-primary col-span-2" />
			{:then result}
				{#if result?.ok}
					<p class="text-lg">Kalender</p>

					{#if result.data.syncEnabled}
						<button on:click={() => toggleCalendarSync(false)} class="btn btn-error"
							>Deaktivieren</button
						>
					{:else}
						<button on:click={() => toggleCalendarSync(true)} class="btn btn-primary"
							>Aktivieren</button
						>
					{/if}
				{/if}
			{/await}
		</div>
	</div>
</div>
