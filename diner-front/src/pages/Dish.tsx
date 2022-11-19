import React, { useState } from 'react';
import { useGetComments, useGetDish, useGetDishResources, useUpdateDish } from '../api/dinerComponents';
import { Link, useLoaderData } from 'react-router-dom';
import { ArrowLeftOutlined, CoffeeOutlined, DollarOutlined, InfoCircleFilled } from '@ant-design/icons';
import { Button, Form, Image, Input, InputNumber, message } from 'antd';
import { useForm } from 'antd/es/form/Form';
import { Comments } from './Comments';

export const dishIdLoader = ({ params }: { params: any }) => {
  return params.id;
};

export const Dish: React.FC = () => {
  const id = useLoaderData() as string;
  const [form] = useForm();
  const [editing, setEditing] = useState(false);
  const comments = useGetComments({ queryParams: { dishId: id } })
  const resources = useGetDishResources({
    queryParams: { id }
  })
  const dish = useGetDish({
    queryParams: {
      id,
    },
  });
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
  const onFinish = ({name, price, description}: { name: string, price: number, description: string}) => {
    updateDish.mutateAsync({
      body: {
        id, name, price, description
      }
    })
  };
  return (
    <div>
      <Link to='/dashboard/menu'>
        {' '}
        <ArrowLeftOutlined /> Go back to menu
      </Link>
      <Form onFinish={onFinish} form={form}>
        <Image width={200} src={'https://joeschmoe.io/api/v1/random'} />
        <br></br>
        <br></br>
        <Button htmlType='submit' onClick={() => {
          setEditing((editing) => !editing);
        }}> {editing ? 'Save Changes' : 'Edit'}</Button>
        <br></br>
        <br></br>
        <Form.Item name={'name'}>
          <Input disabled={!editing} size='large' value={dish.data?.name as string} style={{ width: 250 }}
                 prefix={<CoffeeOutlined />} />
        </Form.Item>
        <Form.Item name={'price'}>
          <InputNumber name={'price'} disabled={!editing} size='large' value={dish.data?.price as number}
                       style={{ width: 250 }} prefix={<DollarOutlined />} />
        </Form.Item>
        <Form.Item name={'description'}>
          <Input.TextArea name={'description'} disabled={!editing} size='large' value={dish.data?.description!}
                          style={{ width: 250 }} prefix={'Info'} />
        </Form.Item>
      </Form>
      <Comments comments={comments.data ? comments.data : [] as []} entity='dish' entityId={id!} />
    </div>
  );
};
