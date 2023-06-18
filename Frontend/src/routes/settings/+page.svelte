<script lang="ts">
	const authProviders = [
		{ name: 'Google', connected: true },
		{ name: 'TickTick', connected: false }
	];
</script>

<div class="flex flex-col gap-y-5">
	<p class="text-3xl font-bold">Einstellungen</p>

	<div class="flex flex-col gap-y-2">
		<p class="text-xl font-bold">Accounts</p>

		{#each authProviders as provider}
			<div class="flex flex-row justify-between">
				<p class="text-lg">{provider.name}</p>
				{#if provider.connected}
					<button class="btn btn-error">Löschen</button>
				{:else}
					<button class="btn btn-primary">Verbinden</button>
				{/if}
			</div>
		{/each}
	</div>
</div>
