import React, {useState} from 'react';
import {Button, Checkbox, Form, FormInstance, Input} from 'antd';
import { useLogIn, useWhoAmI } from '../api/dinerComponents'

export const Auth: React.FC = () => {
  const whoAmIQuery = useWhoAmI({});
  const logInQuery = useLogIn({});
  const formRef = React.createRef<FormInstance>();
  const onReset = () => {
    formRef.current!.resetFields();
  };
  const onFinish = (values: any) => {
    console.log('Success:', values);
  };
  return (
    <main className='auth-page'>
      <h1>Dinner.noSql</h1>
      <Form
        name="basic"
        ref={formRef}
        labelCol={{ span: 8 }}
        wrapperCol={{ span: 16 }}
        onReset={onReset}
        initialValues={{ remember: true }}
        onFinish={onFinish}
        autoComplete="off"
      >
        <Form.Item
          label="Login"
          name="login"
          rules={[{ required: true, message: 'Please input your login!' }]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Password"
          name="password"
          rules={[{ required: true, message: 'Please input your password!' }]}
        >
          <Input.Password />
        </Form.Item>

        <Form.Item name="remember" valuePropName="checked" wrapperCol={{ offset: 8, span: 16 }}>
          <Checkbox>Remember me</Checkbox>
        </Form.Item>

        <Form.Item wrapperCol={{ offset: 8, span: 16 }}>
          <Button type="primary" htmlType="submit" onClick={async () => {
              const login = formRef.current?.getFieldValue("login");
              const password = formRef.current?.getFieldValue("password")
              console.log(login, password);
              const auth = await logInQuery
                  .mutateAsync(
                      {
                          body:
                              {
                                  login,
                                  password
                              }
                      });
          }}>
            Submit
          </Button>
        </Form.Item>
      </Form>
    </main>
  );
}
