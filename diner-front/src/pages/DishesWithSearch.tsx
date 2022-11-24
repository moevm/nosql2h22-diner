import React, { useState } from 'react';
import { useCreateDish, useGetDishes, useGetDishesExcel, useWhoAmI } from '../api/dinerComponents';
import { Dishes } from './Dishes';
import { Dish, DishType } from '../api/dinerSchemas';
import { SearchByName } from './SearchByName';
import { Button, Form, Input, InputNumber, message, Modal, Select, Space } from 'antd';
import FormItem from 'antd/es/form/FormItem';
import { selectDishTypeOptions } from './Dish';

export const DishesWithSearch: React.FC = () => {
	const [searchQuery, setSearchQuery] = useState('');
	const [selectType, setSelectType] = useState('');
	const dishes = useGetDishes({
		queryParams: {
			name: searchQuery,
			dishType: selectType as DishType,
		},
	});
	const getExcel = useGetDishesExcel();
	const createDish = useCreateDish();
	const [isModalOpen, setIsModalOpen] = useState(false);
	const showModal = () => {
		setIsModalOpen(true);
	};
	const handleOk = () => {
		setIsModalOpen(false);
	};
	const handleCancel = () => {
		setIsModalOpen(false);
	};
	const handleSelectChange = (value: DishType) => {
		setSelectType(value);
	};
	const onFinish = ({
		name,
		price,
		description,
	}: {
		name: string;
		price: number;
		description: string;
	}) => {
		createDish
			.mutateAsync({
				body: {
					name,
					price,
					description,
				},
			})
			.then(() => {
				message.success('Created!');
				dishes.refetch();
			});
	};
	const whoAmI = useWhoAmI({});
	const onSearchQueryChange = (value: string) => setSearchQuery(value);
	return (
		<>
			<div>
				<SearchByName onChange={onSearchQueryChange} placeholder={'Dishes search'}></SearchByName>
				<br />
				<Space>
					<Select
						onSelect={handleSelectChange}
						options={selectDishTypeOptions}
						size="large"
						style={{ width: 250 }}
					/>
					<Button onClick={() => setSelectType('')}> Clear </Button>
				</Space>
				<br />
				<br />
				<Space>
					<Button
						loading={whoAmI.isLoading}
						onClick={() =>
							getExcel.mutateAsync({}).then((res) => {
								message.success('Downloaded!');
								let url = window.URL.createObjectURL(res as unknown as Blob);
								let a = document.createElement('a');
								a.href = url;
								a.download = 'dishes.xlsx';
								a.click();
							})
						}
					>
						Export
					</Button>
					<Button
						hidden={!(whoAmI.data?.role === 'Cook' || whoAmI.data?.role === 'Admin')}
						onClick={() => showModal()}
					>
						Add dish
					</Button>
				</Space>
				<Dishes dishes={dishes.data as Dish[]} isLoading={dishes.isLoading}></Dishes>
			</div>
			<Modal
				title="Create dish"
				open={isModalOpen}
				okButtonProps={{ hidden: true }}
				onOk={handleOk}
				onCancel={handleCancel}
			>
				<Form onFinish={onFinish}>
					<FormItem name={'name'} rules={[{ required: true, message: 'Please input name!' }]}>
						<Input placeholder={"Dish's name"} name={'login'}></Input>
					</FormItem>
					<FormItem name={'price'} rules={[{ required: true, message: 'Please input price!' }]}>
						<InputNumber style={{ width: '150px' }} placeholder={"Dish's price"}></InputNumber>
					</FormItem>
					<FormItem
						name={'description'}
						rules={[{ required: true, message: 'Please input description!' }]}
					>
						<Input.TextArea placeholder={"Dish's description"}></Input.TextArea>
					</FormItem>
					<FormItem>
						<Button type={'primary'} htmlType={'submit'}>
							{' '}
							Create{' '}
						</Button>
					</FormItem>
				</Form>
			</Modal>
		</>
	);
};
