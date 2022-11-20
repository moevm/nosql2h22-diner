import React, { useState } from 'react';
import {
	useCreateResource,
	useGetDishes,
	useGetResources,
	useGetResourcesExcel,
	useWhoAmI,
} from '../api/dinerComponents';
import { Dishes } from './Dishes';
import { Dish, Unit } from '../api/dinerSchemas';
import { SearchByName } from './SearchByName';
import { Resources } from './Resources';
import { Button, Form, Input, message, Modal, Select, Space } from 'antd';
import { Link } from 'react-router-dom';
import FormItem from 'antd/es/form/FormItem';

const selectOptions = [
	{
		value: 0,
		label: 'Kg',
	},
	{
		value: 1,
		label: 'Liter',
	},
	{
		value: 2,
		label: 'Items',
	},
];

export const ResourcesWithSearch: React.FC = () => {
	const [searchQuery, setSearchQuery] = useState('');
	const [isModalOpen, setIsModalOpen] = useState(false);
	const resources = useGetResources({
		queryParams: {
			name: searchQuery,
		},
	});
	const getExcel = useGetResourcesExcel({});
	const whoAmI = useWhoAmI({});
	const createResource = useCreateResource();
	const showModal = () => {
		setIsModalOpen(true);
	};
	const handleOk = () => {
		setIsModalOpen(false);
	};
	const handleCancel = () => {
		setIsModalOpen(false);
	};
	const onFinish = ({ name, amount, unit }: { name: string; amount: number; unit: number }) => {
		createResource
			.mutateAsync({
				body: {
					name,
					amount,
					unit: unit as Unit,
				},
			})
			.then((res) => {
				message.success('Resource created!');
				resources.refetch();
			})
			.catch((err) => {
				message.error(err.payload);
			});
	};
	const onSearchQueryChange = (value: string) => setSearchQuery(value);
	return (
		<div>
			<SearchByName onChange={onSearchQueryChange} placeholder={'Dishes search'}></SearchByName>
			<br />
			<Space>
				<Button hidden={!(whoAmI.data?.role === 4)} loading={whoAmI.isLoading} onClick={showModal}>
					Add resource
				</Button>
				<Button
					hidden={!(whoAmI.data?.role === 4)}
					loading={whoAmI.isLoading}
					onClick={() => message.warn('Not implemented yet :)')}
				>
					Import
				</Button>
				<Button
					hidden={!(whoAmI.data?.role === 4 || whoAmI.data?.role === 0)}
					loading={whoAmI.isLoading}
					onClick={() =>
						getExcel.mutateAsync({}).then((res) => {
							message.success('Downloaded!');
							let url = window.URL.createObjectURL(res as unknown as Blob);
							let a = document.createElement('a');
							a.href = url;
							a.download = 'resources.xlsx';
							a.click();
						})
					}
				>
					Export
				</Button>
			</Space>
			<Resources
				label={'Resources found'}
				resources={resources.data as Dish[]}
				isLoading={resources.isLoading}
			></Resources>
			<Modal
				title="Create resource"
				open={isModalOpen}
				okButtonProps={{ hidden: true }}
				onOk={handleOk}
				onCancel={handleCancel}
			>
				<Form onFinish={onFinish}>
					<FormItem name={'name'} rules={[{ required: true, message: 'Please input name!' }]}>
						<Input placeholder={'Name'} name={'name'}></Input>
					</FormItem>
					<FormItem name={'amount'} rules={[{ required: true, message: 'Please input amount!' }]}>
						<Input placeholder={'Amount'} name={'amount'}></Input>
					</FormItem>
					<FormItem name={'unit'} rules={[{ required: true, message: 'Select unit!' }]}>
						<Select options={selectOptions}></Select>
					</FormItem>
					<FormItem>
						<Button type={'primary'} htmlType={'submit'}>
							{' '}
							Create{' '}
						</Button>
					</FormItem>
				</Form>
			</Modal>
		</div>
	);
};
