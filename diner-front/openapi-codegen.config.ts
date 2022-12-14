import { generateSchemaTypes, generateReactQueryComponents } from '@openapi-codegen/typescript';
import { defineConfig } from '@openapi-codegen/cli';
export default defineConfig({
	diner: {
		from: {
			source: 'url',
			url: 'http://localhost:2077/swagger/v1/swagger.json',
		},
		outputDir: 'src/api',
		to: async (context) => {
			const filenamePrefix = 'diner';
			const { schemasFiles } = await generateSchemaTypes(context, {
				filenamePrefix,
			});
			await generateReactQueryComponents(context, {
				filenamePrefix,
				schemasFiles,
			});
		},
	},
});
