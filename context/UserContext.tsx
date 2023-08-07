'use client';

import { useSession } from 'next-auth/react';
import { createContext, ReactNode, useEffect, useMemo, useState } from 'react';
import useSWR from 'swr';
//import { UserType } from 'types';

// interface UserProviderProps {
//   user: UserType; // TODO: Check does UserType fit API response
//   updateUser: (updated_user: UserType) => void;
// };

// Quick fix. TODO: With fresh mind think how to handle initialState, perfectly null
const initialUser = {
  email: '',
  name: '',
  // phoneNumber: '',
};

export const UserContext = createContext({});

export const UserProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState(initialUser);
  const { data: session } = useSession();
  const { data: userDetails } = useSWR(session ? '/api/getUserProfile' : '');

  const proviederValue = useMemo(() => ({ user, updateUser: setUser }), [user, setUser]);

  useEffect(() => {
    setUser(userDetails);
  }, [userDetails]);

  return <UserContext.Provider value={proviederValue}>{children}</UserContext.Provider>;
};
