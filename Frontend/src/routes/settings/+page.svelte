<script lang="ts">
	import { browser } from '$app/environment';
	import { authApi } from '$lib/api-client/clients';

	const getProviders = async () => {
		return await authApi.apiAuthGet();
	};

	const connectProvider = async (name: string) => {
		if (name === 'Google' && browser) {
			const result = await authApi.apiAuthConnectGoogleGet();
			window.location.href = result.url!;
		}
	};

	let promise = getProviders();
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
			<p class="text-lg">Kalender</p>

			<button class="btn btn-primary">Aktivieren</button>
		</div>
	</div>
</div>
