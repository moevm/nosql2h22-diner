import React, { useState } from 'react';
import { Button, Form, Image, Input, message, Select } from 'antd';
import { ArrowLeftOutlined, IdcardOutlined, SmileOutlined, UserOutlined } from '@ant-design/icons';
import { Link, useLoaderData } from 'react-router-dom';
import { useGetUser, useUpdateUser, useWhoAmI } from '../api/dinerComponents';
import { UserRole, UserStatus } from '../api/dinerSchemas';
import { useForm } from 'antd/es/form/Form';

export const userCardIdLoader = ({ params }: { params: any }) => {
	return params.id;
};

const selectRoleOptions = [
	{
		value: 'Admin',
		label: 'Admin',
	},
	{
		value: 'Waiter',
		label: 'Waiter',
	},
	{
		value: 'Manager',
		label: 'Manager',
	},
	{
		value: 'Cook',
		label: 'Cook',
	},
	{
		value: 'Steward',
		label: 'Steward',
	},
];

const selectStatusOptions = [
	{
		value: 'InWork',
		label: 'In Work',
	},
	{
		value: 'NotInWork',
		label: 'Not In Work',
	},
	{
		value: 'Vacation',
		label: 'Vacation',
	},
	{
		value: 'Blocked',
		label: 'Blocked',
	},
];

export const User: React.FC = () => {
	const id = useLoaderData() as string;
	const [form] = useForm();
	const [editing, setEditing] = useState(false);
	const user = useGetUser({ queryParams: { id } });
	const updateUser = useUpdateUser();
	const whoAmI = useWhoAmI({});

	const onFinish = ({
		login,
		fullName,
		role,
		status,
	}: {
		login: string;
		fullName: string;
		role: UserRole;
		status: UserStatus;
	}) => {
		if (!editing)
			updateUser
				.mutateAsync({
					body: {
						id,
						login,
						fullName,
						role,
						status,
					},
				})
				.then(() => message.success('User updated!'));
	};

	React.useEffect(() => {
		if (!id) return;
	}, [id]);
	React.useEffect(() => {
		if (user.isLoading) return;
		if (user.error) {
			console.log(user.error);
			message.error(user.error?.payload);
		}
		if (user.data)
			form.setFieldsValue({
				...user.data,
			});
	}, [user.isLoading, user.isError, user.data]);
	return (
		<div>
			<Link to="/dashboard/staff">
				{' '}
				<ArrowLeftOutlined /> Go back to staff
			</Link>
			<Form onFinish={onFinish} form={form}>
				<Image width={200} src={'https://joeschmoe.io/api/v1/random'} />
				<br></br>
				<br></br>
				<Button
					htmlType="submit"
					onClick={() => {
						setEditing((editing) => !editing);
					}}
					hidden={!(whoAmI.data?.role === 'Admin' || whoAmI.data?.role === 'Manager')}
				>
					{' '}
					{editing ? 'Save Changes' : 'Edit'}
				</Button>
				<br></br>
				<br></br>
				<Form.Item name={'login'}>
					<Input
						disabled={!editing}
						size="large"
						value={user.data?.login as string}
						style={{ width: 250 }}
						prefix={<SmileOutlined />}
					/>
				</Form.Item>
				<Form.Item name={'fullName'}>
					<Input
						name={'fullName'}
						disabled={!editing}
						size="large"
						value={user.data?.fullName as string}
						style={{ width: 250 }}
						prefix={<UserOutlined />}
					/>
				</Form.Item>
				<Form.Item name={'role'}>
					<Select
						options={selectRoleOptions}
						disabled={!editing}
						size="large"
						value={user.data?.role}
						style={{ width: 250 }}
					/>
				</Form.Item>
				<Form.Item name={'status'}>
					<Select
						options={selectStatusOptions}
						disabled={!editing}
						size="large"
						value={user.data?.role}
						style={{ width: 250 }}
					/>
				</Form.Item>
			</Form>
		</div>
	);
};
