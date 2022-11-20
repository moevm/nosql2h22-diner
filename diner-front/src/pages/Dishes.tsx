import { Avatar, Button, List } from 'antd';
import { Link, useNavigate } from 'react-router-dom';
import React from 'react';
import { Dish } from '../api/dinerSchemas';

export const Dishes: React.FC<{ dishes: Dish[]; isLoading: boolean }> = ({ dishes, isLoading }) => {
	const navigate = useNavigate();
	return (
		<div>
			<br />
			<br />
			<List
				itemLayout="horizontal"
				dataSource={dishes}
				loading={isLoading}
				header={<div>Dishes found: {!isLoading ? dishes.length : 0}</div>}
				renderItem={(item) => (
					<List.Item>
						<List.Item.Meta
							avatar={<Avatar src="https://joeschmoe.io/api/v1/random" />}
							title={<a onClick={() => navigate(`/dashboard/menu/${item.id}`)}>{item.name}</a>}
							description={`${item.description}, ${item.price} $`}
						/>
					</List.Item>
				)}
			/>
		</div>
	);
};
