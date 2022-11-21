import React, { useState } from 'react';
import { Button, Card, Form, message, Spin } from 'antd';
import { Link, useLoaderData, useParams } from 'react-router-dom';
import { ArrowLeftOutlined } from '@ant-design/icons';
import { Image, Input, InputNumber } from 'antd';
import { Comments } from './Comments';
import { useGetComments, useGetResource, useUpdateResource } from '../api/dinerComponents';
import { RESOURCES_UNIT } from './Resources';
import FormItem from 'antd/es/form/FormItem';
import { useForm } from 'antd/es/form/Form';

export const resourceIdLoader = ({ params }: { params: any }) => {
	return params.id;
};
export const Resource: React.FC = () => {
	const id = useLoaderData() as string;
	const resource = useGetResource({ queryParams: { id } });
	const comments = useGetComments({ queryParams: { resourceId: id } });
	const updateResource = useUpdateResource();
	const [form] = useForm();
	const onFinish = ({ name, amount }: { name: string; amount: number }) => {
		if (!editing)
			updateResource
				.mutateAsync({
					body: {
						id,
						name,
						amount,
						unit: resource.data?.unit,
					},
				})
				.then(() => message.success('Resource updated!'));
	};
	React.useEffect(() => {
		if (resource.isLoading) return;
		console.log(resource.data);
		if (resource.data) {
			form.setFieldsValue({
				...resource.data,
			});
		}
	}, [resource.isLoading, resource.data]);
	const [editing, setEditing] = useState(false);
	if (resource.isLoading) return <Spin></Spin>;
	return (
		<div>
			<Link to="/dashboard/resources">
				{' '}
				<ArrowLeftOutlined /> Go back to resources
			</Link>
			<br />
			<br />
			<Image width={200} src={'https://joeschmoe.io/api/v1/random'} />
			<br />
			<br />
			<Form form={form} onFinish={onFinish}>
				<FormItem>
					<Button
						htmlType="submit"
						onClick={() => {
							setEditing((editing) => !editing);
						}}
					>
						{' '}
						{editing ? 'Save Changes' : 'Edit'}
					</Button>
				</FormItem>
				<FormItem name={'name'}>
					<Input style={{ width: 250 }} disabled={!editing} />
				</FormItem>
				<FormItem name={'amount'}>
					<InputNumber
						defaultValue={resource?.data ? resource.data?.amount : 0}
						style={{ width: 250 }}
						disabled={!editing}
					/>{' '}
					{resource.data?.unit}
				</FormItem>
			</Form>
			<br />
			<br />
			<Comments comments={comments.data as []} entity="resource" entityId={id!} />
		</div>
	);
};
