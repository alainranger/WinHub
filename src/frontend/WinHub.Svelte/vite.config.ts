import tailwindcss from '@tailwindcss/vite';
import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig, loadEnv } from 'vite';

export default defineConfig(({ mode }) => {
	const env = loadEnv(mode, process.cwd(), '');

	return {
		plugins: [tailwindcss(), sveltekit()],
		server: {
			port: parseInt(env.VITE_PORT),
			proxy: {
				'/api': {
					target: process.env.services__weatherapi__https__0 ||
						process.env.services__weatherapi__http__0,
					changeOrigin: true,
					rewrite: (path) => path.replace(/^\/api/, ''),
					secure: false,
				}
			}
		},
		build: {
			outDir: 'dist',
			rollupOptions: {
				input: './index.html'
			}
		}
	}
})