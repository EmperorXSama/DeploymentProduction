import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    strictPort: true,
    proxy: {
      '/bff': {
        target: 'http://localhost:5006',
        changeOrigin: true,
        secure: false
      }
    }
  }
})