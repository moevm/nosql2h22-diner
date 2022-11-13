import { Button } from 'antd';
import React from 'react';
import { Calendar, Shift } from "./Calendar"

export const Shifts: React.FC = () => {
  const [editMode, setEditMode] = React.useState(false);
  const [shifts, setShifs] = React.useState<Shift[]>([]);

  return <Calendar
    editMode={editMode}
    shifts={shifts}
    onShiftChange={setShifs}
    header={<Button onClick={() => setEditMode(editMode => !editMode)}>{editMode ? 'Save changes' : 'Edit'}</Button>}
  />
}