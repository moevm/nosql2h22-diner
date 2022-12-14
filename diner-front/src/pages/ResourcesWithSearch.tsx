import React, { useState } from 'react';
import {
	fetchImportResources,
	useCreateResource,
	useGetDishes,
	useGetResources,
	useGetResourcesExcel, useGetUsers,
	useImportResources,
	useWhoAmI,
} from '../api/dinerComponents';
import { Dish, Unit } from '../api/dinerSchemas';
import { SearchByName } from './SearchByName';
import { Resources } from './Resources';
import { Button, Form, Input, InputNumber, message, Modal, Select, Space, Upload, UploadProps } from 'antd';
import FormItem from 'antd/es/form/FormItem';
import { RcFile, UploadChangeParam, UploadFile } from 'antd/lib/upload';
import { LoadingOutlined, PlusOutlined } from '@ant-design/icons';
import Search from 'antd/es/input/Search';

const selectOptions = [
	{
		value: 'Kg',
		label: 'Kg',
	},
	{
		value: 'Liter',
		label: 'Liter',
	},
	{
		value: 'Items',
		label: 'Items',
	},
];
const getBase64 = (img: RcFile, callback: (url: string) => void) => {
	const reader = new FileReader();
	reader.addEventListener('load', () => callback(reader.result as string));
	reader.readAsDataURL(img);
};

const beforeUpload = (file: RcFile) => {
	const isXlsx = file.type === 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
	if (!isXlsx) {
		message.error('You can only upload xlsx file!');
	}
	const isLt2M = file.size / 1024 / 1024 < 2;
	if (!isLt2M) {
		message.error('Image must smaller than 2MB!');
	}
	return isXlsx && isLt2M;
};

export const ResourcesWithSearch: React.FC = () => {
	const [searchQuery, setSearchQuery] = useState('');
	const [unitQuery, setUnitQuery] = useState('');
	const [commentQuery, setCommentQuery] = useState('');
	const [amountQuery, setAmountQuery] = useState('');
	const [userIdQuery, setUserIdQuery] = useState('');

	const [isModalOpen, setIsModalOpen] = useState(false);
	const [isUploadModalOpen, setIsUploadModalOpen] = useState(false);
	const resources = useGetResources({
		queryParams: {
			name: searchQuery,
			unit: unitQuery as Unit,
			amount: amountQuery as unknown as number,
			comment: commentQuery,
			userId: userIdQuery,
		},
	});
	const [imageUrl, setImageUrl] = React.useState<string | null>(null);
	const [imageUploading, setImageUploading] = React.useState(false);
	const getExcel = useGetResourcesExcel({});
	const whoAmI = useWhoAmI({});
	const createResource = useCreateResource();
	const importResources = useImportResources({});
	const [userQuerySearch, setUserQuerySearch] = useState('')
	const users = useGetUsers({
		queryParams: {
			nameOrLogin: userQuerySearch,
		}
	});
	const showModal = () => {
		setIsModalOpen(true);
	};
	const showUploadModal = () => {
		setIsUploadModalOpen(true);
	};
	const handleCancelUpload = () => {
		setIsUploadModalOpen(false);
	};
	const handleCancel = () => {
		setIsModalOpen(false);
	};
	const handleOk = () => {
		setIsModalOpen(false);
	};
	const onUserQueryChange = (val: string) => {
		setUserIdQuery(val)
	}
	const onSearch = (val: string) => {
		setUserQuerySearch(val);
	}
	const handleChange: UploadProps['onChange'] = React.useCallback(
		(info: UploadChangeParam<UploadFile>) => {
			if (info.file.status === 'uploading') {
				setImageUploading(true);
				return;
			}
			if (info.file.status === 'done') {
				setImageUploading(false);
				message.success('File saved!');
			}
			if (info.file.status === 'error') {
				setImageUploading(false);
				message.error(info.file.response?.payload);
				// console.log(info.file);
			}
		},
		[],
	);

	const uploadButton = (
		<div>
			{imageUploading ? <LoadingOutlined /> : <PlusOutlined />}
			<div style={{ marginTop: 8 }}>Upload document</div>
		</div>
	);
	const onFinish = ({ name, amount, unit }: { name: string; amount: number; unit: Unit }) => {
		createResource
			.mutateAsync({
				body: {
					name,
					amount,
					unit: unit,
				},
			})
			.then((res) => {
				message.success('Resource created!');
				resources.refetch();
			})
			.catch((err) => {
				// message.error(err.payload);
			});
	};
	const onSearchQueryChange = (value: string) => setSearchQuery(value);
	return (
		<div>
			<Modal
				title="Import resources"
				open={isUploadModalOpen}
				okButtonProps={{ hidden: true }}
				onCancel={handleCancelUpload}
			>
				<Upload
					name="avatar"
					listType="picture-card"
					className="avatar-uploader"
					showUploadList={false}
					beforeUpload={beforeUpload}
					onChange={handleChange}
					action={'/api/resource/import-resources'}
				>
					{imageUrl ? <img src={imageUrl} alt="avatar" style={{ width: '100%' }} /> : uploadButton}
				</Upload>
			</Modal>
			<SearchByName onChange={onSearchQueryChange} placeholder={'Resources search'}></SearchByName>
			<br />
			<Space>
			<Space>
				Unit type: <Select onChange={(value) => setUnitQuery(value)} options={selectOptions} size={"large"} style={{width: '100px'}}></Select>
				<Button onClick={() => setUnitQuery('')}> Clear </Button>
			</Space>
			<br />
			<Space>
				Amount left: <InputNumber onChange={(value) => setAmountQuery(value as string)} size={"large"} style={{width: '100px'}}></InputNumber>
				<Button onClick={() => setAmountQuery('')}> Clear </Button>
			</Space>
			<br />
			Commented by: <Select
				showSearch
				placeholder="Select a user"
				optionFilterProp="children"
				onChange={onUserQueryChange}
				onSearch={onSearch}
				filterOption={(input, option) =>
					(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
				}
				options={users.data?.map((x) => {
					return {
						label: x.login,
						value: x.id,
					};
				})}
			/><Button onClick={() => onUserQueryChange('')}> Clear </Button>
			<br />
			</Space>
			<br />
			<br />
			<Search
				size={'large'}
				onSearch={(value) => setCommentQuery(value)}
				enterButton={'Search'}
				style={{ width: 'calc(100% - 350px)' }}
				placeholder={"Comment message"}
			/>
			<Space>
				<br />
				<Button
					hidden={!(whoAmI.data?.role === 'Steward' || whoAmI.data?.role === 'Admin')}
					loading={whoAmI.isLoading}
					onClick={showModal}
				>
					Add resource
				</Button>
				<Button
					hidden={!(whoAmI.data?.role === 'Steward' || whoAmI.data?.role === 'Admin')}
					loading={whoAmI.isLoading}
					onClick={() => showUploadModal()}
				>
					Import
				</Button>
				<Button
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
