<script lang="ts">
	import { signIn } from '@auth/sveltekit/client';
</script>

<div class="hero min-h-screen bg-base-200">
	<div class="hero-content flex-col lg:flex-row-reverse">
		<div class="text-center lg:text-left">
			<h1 class="text-5xl font-bold">Melde dich an</h1>
			<p class="py-6">
				Um BoarDo benutzen zu können, melde dich bitte zuerst mit deinem Google Accout an.
			</p>
		</div>
		<div class="card flex-shrink-0 w-full max-w-sm shadow-2xl bg-base-100">
			<div class="card-body">
				<button class="btn" on:click={() => signIn()}>Mit Google anmelden</button>
			</div>
		</div>
	</div>
</div>
