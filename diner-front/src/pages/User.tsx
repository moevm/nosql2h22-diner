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
		value: 0,
		label: 'Admin',
	},
	{
		value: 1,
		label: 'Waiter',
	},
	{
		value: 2,
		label: 'Manager',
	},
	{
		value: 3,
		label: 'Cook',
	},
	{
		value: 4,
		label: 'Steward',
	},
];

const selectStatusOptions = [
	{
		value: 0,
		label: 'In Work',
	},
	{
		value: 1,
		label: 'Not In Work',
	},
	{
		value: 2,
		label: 'Vacation',
	},
	{
		value: 3,
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
					hidden={!(whoAmI.data?.role === 0 || whoAmI.data?.role === 2)}
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
