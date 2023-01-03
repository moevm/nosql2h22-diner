import React, { useState } from 'react';
import { Avatar, Button, Form, Input, InputNumber, List, message, Modal, Select } from 'antd';
import FormItem from 'antd/es/form/FormItem';
import { useGetResources, useUpdateDish } from '../api/dinerComponents';
import { RESOURCES_UNIT } from './Resources';
import { Link } from 'react-router-dom';
import { Dish, Resource } from '../api/dinerSchemas';
import { CloseOutlined } from '@ant-design/icons';

export const EditDishResources: React.FC<{
	isModalOpen: boolean;
	handleOk: () => void;
	handleCancel: () => void;
	dishResources: Resource[];
	isLoading: boolean;
	dish: Dish;
	onUpdate: () => void;
}> = ({ isModalOpen, handleOk, handleCancel, dishResources, isLoading, dish, onUpdate }) => {
	const [searchQuery, setSearchQuery] = useState('');
	const resources = useGetResources({});
	const [dishResourcesList, setDishResourcesList] = useState(dishResources ?? null);
	const updateDishResource = useUpdateDish({});
	const onFinish = () => {
		updateDishResource
			.mutateAsync({
				body: {
					id: dish.id,
					name: dish.name,
					description: dish.description,
					price: dish.price,
					listDishResourceDtos: dishResourcesList.map((x) => {
						return {
							resourceId: x.id,
							required: x.amount,
						};
					}),
				},
			})
			.then(() => {
				message.success('Dish updated!');
				onUpdate();
				Modal.destroyAll();
			})
			.catch((err) => message.error(err?.payload));
	};
	const onSearch = React.useCallback(
		(val: string) => {
			setSearchQuery(searchQuery);
		},
		[searchQuery],
	);
	const onChange = (val: string) => {
		setDishResourcesList((arr: Resource[]) => {
			if (arr.some((x) => x.id === val)) {
				return arr;
			}
			const newArr = [...arr, resources.data?.find((x) => x.id === val) as Resource];
			return newArr;
		});
	};
	React.useEffect(() => {
		if (!dishResources) return;
		if (isLoading) return;
		if (dishResourcesList === null) setDishResourcesList(dishResources);
	}, [dishResourcesList, dishResources, isLoading]);
	return (
		<Modal
			title="Edit resource"
			open={isModalOpen}
			okButtonProps={{ hidden: true }}
			onOk={handleOk}
			onCancel={handleCancel}
		>
			<Form onFinish={onFinish}>
				<FormItem>
					<Select
						showSearch
						placeholder="Select a resource"
						optionFilterProp="children"
						onChange={onChange}
						onSearch={onSearch}
						filterOption={(input, option) =>
							(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
						}
						options={resources.data?.map((x) => {
							return {
								label: x.name,
								value: x.id,
							};
						})}
					/>
				</FormItem>
				<FormItem>
					<Button type={'primary'} htmlType={'submit'}>
						{' '}
						Update{' '}
					</Button>
				</FormItem>
			</Form>
			<List
				loading={isLoading}
				size="large"
				header={
					<div>
						{'Resources selected'}: {dishResourcesList?.length ? dishResourcesList?.length : 0}
					</div>
				}
				dataSource={dishResourcesList}
				renderItem={(item) => (
					<List.Item>
						<List.Item.Meta
							avatar={<Avatar src={'https://joeschmoe.io/api/v1/randoma'} />}
							title={item.name}
							description={`#${item.id}, ${item.amount} ${item.unit}`}
						/>
						<InputNumber
							value={item.amount}
							onChange={(val) => {
								setDishResourcesList((arr: Resource[]) => {
									const newArr = arr.map((x) => {
										if (x.id === item.id)
											return {
												...x,
												amount: val as number,
											};
										else return x;
									});
									return newArr;
								});
							}}
						></InputNumber>
						<Button
							type="primary"
							icon={<CloseOutlined />}
							onClick={() => {
								setDishResourcesList((arr) => {
									const newArr = arr.filter((x) => x.id !== item.id);
									return newArr;
								});
							}}
						/>
					</List.Item>
				)}
			/>
		</Modal>
	);
};
