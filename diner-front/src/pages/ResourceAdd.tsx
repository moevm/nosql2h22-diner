import React from 'react'
import { Button, Card, UploadProps } from 'antd';
import { Link, useParams } from 'react-router-dom';
import { ArrowLeftOutlined, LoadingOutlined, PlusOutlined } from '@ant-design/icons';
import { Image, Input, InputNumber, Upload, message } from 'antd';
import { RcFile, UploadChangeParam, UploadFile } from 'antd/lib/upload';

const getBase64 = (img: RcFile, callback: (url: string) => void) => {
  const reader = new FileReader();
  reader.addEventListener('load', () => callback(reader.result as string));
  reader.readAsDataURL(img);
};

const beforeUpload = (file: RcFile) => {
  const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png';
  if (!isJpgOrPng) {
    message.error('You can only upload JPG/PNG file!');
  }
  const isLt2M = file.size / 1024 / 1024 < 2;
  if (!isLt2M) {
    message.error('Image must smaller than 2MB!');
  }
  return isJpgOrPng && isLt2M;
};

export const ResourceAdd: React.FC = () => {
  const [imageUrl, setImageUrl] = React.useState<string | null>(null)
  const [imageUploading, setImageUploading] = React.useState(false);
  const [name, setName] = React.useState('');
  const [amount, setAmount] = React.useState<number | null>(0);

  const handleChange: UploadProps['onChange'] = React.useCallback((info: UploadChangeParam<UploadFile>) => {
    if (info.file.status === 'uploading') {
      setImageUploading(true);
      return;
    }
    if (info.file.status === 'done') {
      // Get this url from response in real world.
      getBase64(info.file.originFileObj as RcFile, url => {
        setImageUploading(false);
        setImageUrl(url);
      });
    }
  }, [])

  const uploadButton = (
    <div>
      {imageUploading ? <LoadingOutlined /> : <PlusOutlined />}
      <div style={{ marginTop: 8 }}>Upload image</div>
    </div>
  );

  return <div>
    <Link to="/dashboard/resources"> <ArrowLeftOutlined /> Go back to resources</Link>
    <br />
    <br />
    <Upload
      name="avatar"
      listType="picture-card"
      className="avatar-uploader"
      showUploadList={false}
      action="https://www.mocky.io/v2/5cc8019d300000980a055e76"
      beforeUpload={beforeUpload}
      onChange={handleChange}
    >
      {imageUrl ? <img src={imageUrl} alt="avatar" style={{ width: '100%' }} /> : uploadButton}
    </Upload>
    <br />
    <br />
    <Card title={`New resource`} style={{ width: 501 }}>
      <Card.Grid style={{ width: 250 }}>Name</Card.Grid>
      <Card.Grid style={{ width: 250 }}><Input value={name} onChange={(event) => setName(event.target.value)} /></Card.Grid>
      <Card.Grid style={{ width: 250 }}>Amount</Card.Grid>
      <Card.Grid style={{ width: 250 }}><InputNumber value={amount} onChange={(setAmount)} /> kg</Card.Grid>
    </Card>
    <br />
    <br />
    <Button type="primary">Add resource</Button>
  </div>
}