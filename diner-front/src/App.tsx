import {
  createBrowserRouter,
  RouterProvider,
  Route,
} from "react-router-dom";
import { Auth } from './pages/Auth'
import { Dashboard } from './pages/Dashboard'
import { Shifts } from './pages/Shifts'
import { ErrorPage } from './pages/Error';
import { Payments } from './pages/Payments';
import { Payment } from './pages/Payment';
import { Resources } from './pages/Resources';
import { Resource } from './pages/Resource';
import { ResourceAdd } from './pages/ResourceAdd';
import { useWhoAmI } from "./api/dinerComponents";
import { Button, Result, Spin } from "antd";

const router = createBrowserRouter([
  {
    path: "/",
    element: <div>Tbd</div>,
    errorElement: <ErrorPage />,
  },
  {
    path: "/auth",
    element: <Auth />,
    errorElement: <ErrorPage />,
  },
  {
    path: "/dashboard",
    element: <Dashboard />,
    errorElement: <ErrorPage />,
    children: [
      {
        path: "shifts",
        element: <Shifts />
      },
      {
        path: "shifts/edit",
        element: 'shifts editor',
      },
      {
        path: "shifts/search",
        element: 'shifts search',
      },
      {
        path: "menu",
        element: 'menu',
      },
      {
        path: "menu/item/:id",
        element: 'menu item',
      },
      {
        path: "menu/item/:id/edit",
        element: 'menu item edit',
      },
      {
        path: "menu/item/:id/edit/resources",
        element: 'menu item resources edit',
      },
      {
        path: "menu/item/:id/comment",
        element: 'menu item comment',
      },
      {
        path: "staff",
        element: 'staff',
      },
      {
        path: "staff/person/:id",
        element: 'staff person',
      },
      {
        path: "staff/add",
        element: 'staff add',
      },
      {
        path: "resources",
        element: <Resources />
      },
      {
        path: "resources/resource/:id",
        element: <Resource />
      },
      {
        path: "resources/add",
        element: <ResourceAdd />
      },
      {
        path: "payments",
        element: <Payments />
      },
      {
        path: "payments/payment/:id",
        element: <Payment />
      },
    ],
  },
]);

export const App: React.FC = () => {
  const whoAmI = useWhoAmI({});

  if (whoAmI.isLoading) {
    return <div className='app-root'><Spin /></div>
  }

  if (whoAmI.error) {
    return <div id="error-page">
      <Result
        status={whoAmI.error.status}
        title="Error"
        subTitle={whoAmI.error.statusText || whoAmI.error.message}
        extra={<Button type="primary" onClick={() => whoAmI.refetch()}>Try again</Button>}
      />
    </div>
  }


  return (
    <div className='app-root'>
      <RouterProvider router={router} />
    </div>
  )
}
