import React from 'react';
import { Button, Dropdown, message, PageHeader } from 'antd';
import { Avatar } from 'antd';
import {
	CalendarOutlined,
	UserOutlined,
	CoffeeOutlined,
	TeamOutlined,
	DollarCircleOutlined,
	GoldOutlined,
} from '@ant-design/icons';
import { Menu } from 'antd';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import { useLogOut, useWhoAmI } from '../api/dinerComponents';

export const Dashboard: React.FC = () => {
	const navigate = useNavigate();
	const menuItems = React.useMemo(
		() => [
			{
				label: 'Shifts',
				key: 'shifts',
				icon: <CalendarOutlined />,
				onClick: () => navigate('/dashboard/shifts'),
			},
			{
				label: 'Menu',
				key: 'menu',
				icon: <CoffeeOutlined />,
				onClick: () => navigate('/dashboard/menu'),
			},
			{
				label: 'Staff',
				key: 'staff',
				icon: <TeamOutlined />,
				onClick: () => navigate('/dashboard/staff'),
			},
			{
				label: 'Payments',
				key: 'payments',
				icon: <DollarCircleOutlined />,
				onClick: () => navigate('/dashboard/payments'),
			},
			{
				label: 'Resources',
				key: 'resources',
				icon: <GoldOutlined />,
				onClick: () => navigate('/dashboard/resources'),
			},
		],
		[],
	);
	const user = useWhoAmI({});
	const logout = useLogOut();

	const userDropdownMenu = React.useMemo(
		() => ({
			items: [
				{
					key: 'my-shift',
					label: (
						<Link to={`/dashboard/shifts/${user.data?.id}`}>
							<Button> My shift</Button>
						</Link>
					),
				},
				{
					key: 'my-card',
					label: (
						<Link to={`/dashboard/staff/${user.data?.id}`}>
							<Button> My card</Button>
						</Link>
					),
				},
				{
					key: 'logout',
					label: (
						<Button
							onClick={() =>
								logout
									.mutateAsync({})
									.then(() => navigate('/auth'))
									.catch((err) => message.error(err.payload))
							}
							loading={logout.isLoading}
						>
							Log out
						</Button>
					),
				},
			],
		}),
		[user.data, logout.isLoading, logout.isSuccess],
	);

	React.useEffect(() => {
		if (logout.error) {
			message.error(logout.error?.payload);
		}
		if (!user.data || user.isError) {
			navigate('/auth');
		}
	}, [logout.error, user.isError, user.data]);

	return (
		<div>
			<header>
				<PageHeader
					className="page-header"
					title={<a href={'https://github.com/moevm/nosql2h22-diner'}> Diner.noSql</a>}
					extra={[
						<Dropdown key="avatar" menu={userDropdownMenu}>
							<div>
								<Avatar icon={<UserOutlined />} /> {user.data?.fullName}
							</div>
						</Dropdown>,
					]}
				/>
			</header>
			<div className="page-wrapper">
				<Menu items={menuItems} className="navigation" />
				<main className="page-content">
					<Outlet />
				</main>
			</div>
		</div>
	);
};
