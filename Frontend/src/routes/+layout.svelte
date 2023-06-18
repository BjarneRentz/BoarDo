<script>
	import '../app.css';

	const closeDrawer = () => {
		document.getElementById('my-drawer')?.click();
	};
</script>

<div class="drawer">
	<input id="my-drawer" type="checkbox" class="drawer-toggle" />
	<div class="drawer-content">
		<!-- Page content here -->

		<div class="navbar bg-base-100">
			<div class="flex-none">
				<label for="my-drawer" class="btn btn-square btn-ghost drawer-button">
					<svg
						xmlns="http://www.w3.org/2000/svg"
						fill="none"
						viewBox="0 0 24 24"
						class="inline-block w-5 h-5 stroke-current"
						><path
							stroke-linecap="round"
							stroke-linejoin="round"
							stroke-width="2"
							d="M4 6h16M4 12h16M4 18h16"
						/></svg
					>
				</label>
			</div>
			<div class="flex-1">
				<a href="/" class="btn btn-ghost normal-case text-xl">BoarDo</a>
			</div>
		</div>
		<div class="px-8">
			<slot />
		</div>
	</div>
	<div class="drawer-side">
		<label for="my-drawer" class="drawer-overlay" />
		<ul class="menu p-4 w-80 h-full bg-base-200 text-base-content">
			<!-- Sidebar content here -->
			<li><a href="/" on:click={closeDrawer}>Home</a></li>
			<li><a href="/settings" on:click={closeDrawer}>Settings</a></li>
		</ul>
	</div>
</div>
