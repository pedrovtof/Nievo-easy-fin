import React from 'react';
import Sidebar from '../Sidebar';
import Header from '../Header';
import { LayoutContainer, MainContent, ContentArea } from './styles';

const LayoutView = ({ children }) => {
  return (
    <LayoutContainer>
      <Sidebar />
      <MainContent>
        <Header />
        <ContentArea>
          {children}
        </ContentArea>
      </MainContent>
    </LayoutContainer>
  );
};

export default LayoutView;
