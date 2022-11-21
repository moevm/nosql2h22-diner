import React from 'react';
import { Input, DatePicker, Button, List, Avatar, Space } from 'antd';
import { Link } from 'react-router-dom';
import { useGetResources } from '../api/dinerComponents';
import { Resource } from '../api/dinerSchemas';

export const RESOURCES_UNIT = ['kg', 'liters', 'items'];

export const Resources: React.FC<{ resources: Resource[]; isLoading: boolean; label: string }> = ({
	resources,
	isLoading,
	label,
}) => {
	React.useEffect(() => {
		if (isLoading) return;
	}, [isLoading]);
	return (
		<div>
			<List
				size="large"
				header={
					<div>
						{label}: {isLoading ? 0 : resources?.length}
					</div>
				}
				dataSource={resources}
				renderItem={(item) => (
					<Link key={item.id} to={`/dashboard/resources/${item.id}`}>
						{' '}
						<List.Item>
							<List.Item.Meta
								avatar={<Avatar src={'https://joeschmoe.io/api/v1/randoma'} />}
								title={item.name}
								description={`#${item.id}, ${item.amount} ${item.unit}`}
							/>
						</List.Item>
					</Link>
				)}
			/>
		</div>
	);
};
