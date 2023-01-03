import React, { useState } from 'react';
import {
	useGetComments,
	useGetDish,
	useGetDishResources,
	useUpdateDish,
	useWhoAmI,
} from '../api/dinerComponents';
import { Link, useLoaderData } from 'react-router-dom';
import {
	ArrowLeftOutlined,
	CoffeeOutlined,
	DollarOutlined,
	InfoCircleFilled,
} from '@ant-design/icons';
import { Button, Form, Image, Input, InputNumber, message, Select, Space } from 'antd';
import { useForm } from 'antd/es/form/Form';
import { Comments } from './Comments';
import { Resources } from './Resources';
import { Dish as DishEntity, DishType, Resource } from '../api/dinerSchemas';
import { EditDishResources } from './EditDishResources';

export const dishIdLoader = ({ params }: { params: any }) => {
	return params.id;
};

export const selectDishTypeOptions = [
	{
		value: 'Soup',
		label: 'Soup',
	},
	{
		value: 'Snack',
		label: 'Snack',
	},
	{
		value: 'Bar',
		label: 'Bar',
	},
	{
		value: 'Hot',
		label: 'Hot',
	},
];

export const Dish: React.FC = () => {
	const id = useLoaderData() as string;
	const [form] = useForm();
	const [editing, setEditing] = useState(false);
	const [isModalOpen, setIsModalOpen] = useState(false);
	const comments = useGetComments({ queryParams: { dishId: id } });
	const resources = useGetDishResources({
		queryParams: { id },
	});
	const whoAmI = useWhoAmI({});
	const dish = useGetDish({
		queryParams: {
			id,
		},
	});
	const handleOk = () => {
		setIsModalOpen(false);
	};
	const handleCancel = () => {
		setIsModalOpen(false);
	};
	const updateDish = useUpdateDish();
	React.useEffect(() => {
		if (dish.isLoading) return;
		if (dish.error) {
			console.log(dish.error);
			message.error(dish.error?.payload);
		}
		if (dish.data) {
			console.log(dish.data);
			form.setFieldsValue({
				...dish.data,
			});
		}
		console.log(resources.data);
	}, [dish.isLoading, dish.isError, dish.data, resources.data]);
	const onFinish = ({
		name,
		price,
		description,
		dishType,
	}: {
		name: string;
		price: number;
		description: string;
		dishType: DishType;
	}) => {
		updateDish
			.mutateAsync({
				body: {
					id,
					name,
					price,
					description,
					dishType,
					listDishResourceDtos: resources.data?.map((x) => {
						return {
							resourceId: x.id,
							required: x.amount,
						};
					}),
				},
			})
			.then(() => message.success('Updated!'));
	};
	return (
		<div>
			<Link to="/dashboard/menu">
				{' '}
				<ArrowLeftOutlined /> Go back to menu
			</Link>
			<Space style={{ display: 'flex', flexDirection: 'row' }}>
				<Form onFinish={onFinish} form={form}>
					<Image width={200} src={'https://joeschmoe.io/api/v1/random'} />
					<br></br>
					<br></br>
					<Button
						htmlType="submit"
						hidden={
							!(
								whoAmI.data?.role === 'Cook' ||
								whoAmI.data?.role === 'Manager' ||
								whoAmI.data?.role === 'Admin'
							)
						}
						onClick={() => {
							setEditing((editing) => !editing);
						}}
					>
						{' '}
						{editing ? 'Save Changes' : 'Edit'}
					</Button>
					<br></br>
					<br></br>
					<Form.Item name={'name'}>
						<Input
							disabled={!editing}
							size="large"
							value={dish.data?.name as string}
							style={{ width: 250 }}
							prefix={<CoffeeOutlined />}
						/>
					</Form.Item>
					<Form.Item name={'price'}>
						<InputNumber
							name={'price'}
							disabled={!editing}
							size="large"
							value={dish.data?.price as number}
							style={{ width: 250 }}
							prefix={<DollarOutlined />}
						/>
					</Form.Item>
					<Form.Item name={'dishType'}>
						<Select
							options={selectDishTypeOptions}
							value={dish.data?.dishType}
							disabled={!editing}
							size="large"
							style={{ width: 250 }}
						/>
					</Form.Item>
					<Form.Item name={'description'}>
						<Input.TextArea
							name={'description'}
							disabled={!editing}
							size="large"
							value={dish.data?.description!}
							style={{ width: 250 }}
							prefix={'Info'}
						/>
					</Form.Item>
				</Form>
				<div style={{ flexDirection: 'column', display: 'flex', alignItems: 'center' }}>
					<div style={{ overflow: 'scroll', height: '400px', width: '400px' }}>
						<Resources
							label={'Resources that is used'}
							resources={resources.data as Resource[]}
							isLoading={resources.isLoading}
						/>
					</div>
					<Button
						hidden={
							!(
								whoAmI.data?.role === 'Manager' ||
								whoAmI.data?.role === 'Cook' ||
								whoAmI.data?.role === 'Admin'
							)
						}
						style={{ width: '250px' }}
						htmlType="submit"
						onClick={() => {
							resources.refetch();
							setIsModalOpen((open) => !open);
						}}
					>
						{' '}
						Edit resources
					</Button>
				</div>
			</Space>
			<Comments
				comments={comments.data ? comments.data : ([] as [])}
				entity="dish"
				entityId={id!}
			/>
			<EditDishResources
				isModalOpen={isModalOpen}
				handleOk={handleOk}
				handleCancel={handleCancel}
				dishResources={resources.data as []}
				isLoading={resources.isLoading}
				dish={dish.data as DishEntity}
				onUpdate={() => resources.refetch()}
			></EditDishResources>
		</div>
	);
};
