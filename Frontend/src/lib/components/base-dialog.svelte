<script lang="ts">
	export let visible = false;
</script>

<div class="modal modal-middle sm:modal-middle" class:modal-open={visible}>
	<slot />
</div>
