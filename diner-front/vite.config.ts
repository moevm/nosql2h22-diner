import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    host: '0.0.0.0',
    port: 2077,
    proxy: {
      "/api": "http://0.0.0.0:5231",
      "/swagger": "http://0.0.0.0:5231",
    },
  },
});
