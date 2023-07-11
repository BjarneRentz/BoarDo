<script lang="ts">
	import { browser } from '$app/environment';
	import { authApi, calendarApi } from '$lib/api-client/clients';

	const getProviders = async () => {
		return await authApi.apiAuthGet();
	};

	const connectProvider = async (name: string) => {
		if (name === 'Google' && browser) {
			const result = await authApi.apiAuthConnectGoogleGet();
			window.location.href = result.url!;
		}
	};

	const toggleCalendarSync = async (enable: boolean) => {
		await calendarApi.apiCalendarSyncEnablePost({ enable });
		getCalendarSyncState = calendarApi.apiCalendarSyncStateGet();
	};

	let promise = getProviders();

	let getCalendarSyncState = calendarApi.apiCalendarSyncStateGet();
</script>

<div class="flex flex-col gap-y-5">
	<p class="text-3xl font-bold">Einstellungen</p>

	<div class="flex flex-col gap-y-2">
		<p class="text-xl font-bold">Accounts</p>

		<div class="grid grid-cols-2 gap-8">
			{#await promise}
				<progress class="progress progress-primary col-span-2" />
			{:then result}
				{#each Object.entries(result) as [key, value]}
					<p class="text-lg">{key}</p>
					{#if value}
						<button class="btn btn-error">Löschen</button>
					{:else}
						<button on:click={() => connectProvider(key)} class="btn btn-primary">Verbinden</button>
					{/if}
				{/each}
			{/await}
		</div>
	</div>

	<div class="flex flex-col gap-y-2">
		<p class="text-xl font-bold">Sync</p>

		<div class="grid grid-cols-2 gap-8">
			{#await getCalendarSyncState}
				<progress class="progress progress-primary col-span-2" />
			{:then result}
				<p class="text-lg">Kalender</p>

				{#if result.syncEnabled}
					<button on:click={() => toggleCalendarSync(false)} class="btn btn-error"
						>Deaktivieren</button
					>
				{:else}
					<button on:click={() => toggleCalendarSync(true)} class="btn btn-primary"
						>Aktivieren</button
					>
				{/if}
			{/await}
		</div>
	</div>
</div>
