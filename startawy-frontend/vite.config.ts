import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      // Frontend calls /api/* and Vite proxies to ASP.NET locally.
      '/api': {
        target: 'http://localhost:5150',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})
