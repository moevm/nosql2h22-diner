import { useRouteError, Link } from 'react-router-dom';
import React from 'react';
import { Button, Result } from 'antd';

export const ErrorPage: React.FC = () => {
	const error: any = useRouteError();
	console.error(error);

	return (
		<div id="error-page">
			<Result
				status={error.status}
				title="Error"
				subTitle={error.statusText || error.message}
				extra={
					<Link to="/">
						<Button type="primary">Back Home</Button>
					</Link>
				}
			/>
		</div>
	);
};
