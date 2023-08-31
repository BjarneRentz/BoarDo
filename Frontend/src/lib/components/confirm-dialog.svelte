<script lang="ts">
	import BaseDialog from './base-dialog.svelte';

	export let confirmVisible = false;
	export let title = '';
	export let confirmText = 'Ok';
	export let cancelText = 'Cancel';

	export let cancelCallback: Function | null = null;
	export let okCallback: Function | null;

	const cancel = () => {
		confirmVisible = false;

		if (cancelCallback) {
			cancelCallback();
		}
	};

	const ok = () => {
		confirmVisible = false;

		if (okCallback) {
			okCallback();
		}
	};
</script>

<BaseDialog visible={confirmVisible}>
	<div class="modal-box">
		<h3 class="text-lg">{title}</h3>

		<div class="modal-action">
			<button class="btn btn-error" on:click={() => ok()}>{confirmText}</button>
			<button class="btn" on:click={() => cancel()}>{cancelText}</button>
		</div>
	</div>
</BaseDialog>
