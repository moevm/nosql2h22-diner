import React, { useState } from 'react';
import { useCreateUser, useGetUsers } from '../api/dinerComponents';
import UserList from './Users';
import { User } from '../api/dinerSchemas';
import { SearchByName } from './SearchByName';
import { Button, Form, Input, message, Modal, Space } from 'antd';
import FormItem from 'antd/es/form/FormItem';

export const UsersWithSearch: React.FC = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const users = useGetUsers({ queryParams: { nameOrLogin: searchQuery } });
  const createUser = useCreateUser();
  const onSearchQueryChange = (value: string) => setSearchQuery(value);
  const showModal = () => {
    setIsModalOpen(true);
  };
  const handleOk = () => {
    setIsModalOpen(false);
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };
  React.useEffect(() => {
    if (createUser.error || users.error) {
      console.log(createUser.error);
      message.error(createUser.error?.payload || users.error?.payload);
    }
  }, [createUser.error, users.error]);
  const onFinish = React.useCallback(
    ({ login, fullName, password }: { login: string, fullName: string, password: string }) => {
      createUser.mutateAsync({ body: { login, fullName, password } }).then(res => {
        if (res) {
          message.success('User created!');
          console.log('Created!');
        }
      }).catch(err => {
        console.log(err);
      });
    }, [createUser.isSuccess, createUser.isError],
  );
  React.useEffect(() => {
    if (users.isLoading) return;
  }, [users.isLoading]);
  return (
    <div>
      <Input.Group>
        <SearchByName onChange={onSearchQueryChange} placeholder={"Users search"} />
      </Input.Group>
      <br />
      <Space>
        <Button onClick={showModal}>Add Staff</Button>
      </Space>
      <UserList users={users.data as User[]} isLoading={users.isLoading} />
      <Modal title='Create user' open={isModalOpen} okButtonProps={{ hidden: true }} onOk={handleOk}
             onCancel={handleCancel}>
        <Form onFinish={onFinish}>
          <FormItem
            name={'login'}
            rules={[{ required: true, message: 'Please input login!' }]}
          >
            <Input placeholder={'User\'s login'} name={'login'}></Input>
          </FormItem>
          <FormItem
            name={'fullName'}
            rules={[{ required: true, message: 'Please input full name!' }]}
          >
            <Input placeholder={'User\'s full name'} name={'fullName'}></Input>
          </FormItem>
          <FormItem
            name={'password'}
            rules={[{ required: true, message: 'Please password!' }]}
          >
            <Input placeholder={'User\'s password'} name={'password'}></Input>
          </FormItem>
          <FormItem>
            <Button type={'primary'} htmlType={'submit'}> Create </Button>
          </FormItem>
        </Form>
      </Modal>
    </div>
  );
};
