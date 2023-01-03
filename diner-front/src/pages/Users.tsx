import { Avatar, Button, List } from 'antd';
import React from 'react';
import { User } from '../api/dinerSchemas';
import { Link, useNavigate } from 'react-router-dom';
const App: React.FC<{ users: User[]; isLoading: boolean }> = ({ users, isLoading }) => {
	const navigate = useNavigate();
	const userRoles: string[] = React.useMemo(() => {
		return ['Admin', 'Waiter', 'Manager', 'Cook', 'Steward'];
	}, []);
	return (
		<div>
			<br />
			<br />
			<List
				itemLayout="horizontal"
				dataSource={users}
				loading={isLoading}
				header={<div>Users found: {!isLoading ? users.length : 0}</div>}
				renderItem={(item) => (
					<List.Item>
						<List.Item.Meta
							avatar={<Avatar src="https://joeschmoe.io/api/v1/random" />}
							title={<a onClick={() => navigate(`/dashboard/staff/${item.id}`)}>{item.fullName}</a>}
							description={`${item.login}, ${item.role}`}
						/>
						<Link to={`/dashboard/shifts/${item.id}`}>
							<Button>Shifts</Button>
						</Link>
					</List.Item>
				)}
			/>
		</div>
	);
};

export default App;
