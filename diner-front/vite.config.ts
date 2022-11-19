import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

console.log(`http://${process.env.API ?? '0.0.0.0'}:${process.env.PORT ?? '5231'}`);
// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    host: '0.0.0.0',
    port: 2077,
    proxy: {
      "/api": { 
        target: `http://${process.env.API ?? '0.0.0.0'}:${process.env.PORT ?? '5231'}` 
      },
      "/swagger": { 
        target: `http://${process.env.API ?? '0.0.0.0'}:${process.env.PORT ?? '5231'}` 
      },
    },
  },
});
