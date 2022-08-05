import React from 'react';
import { Routes, Route, Link } from 'react-router-dom';
import Create from './Create';
import List from './List';
import {
  ChakraProvider,
  Box,
  Container,
  VStack,
  Code,
  Grid,
  theme,
} from '@chakra-ui/react';
import { ColorModeSwitcher } from './ColorModeSwitcher';
import { Logo } from './Logo';

function App() {
  return (
    <ChakraProvider theme={theme}>
      <Container maxW="container.xl">
        <Box textAlign="center" fontSize="xl">
          <h1>Welcome to Splashtest!</h1>

          <Link to="/create">Create new test</Link>
          <Link to="/list">Check all the tests</Link>

          <Box p={5}>
            <Routes>
              <Route path="create" element={<Create />} />
              <Route path="list" element={<List />} />
            </Routes>
          </Box>
        </Box>
      </Container>
    </ChakraProvider>
  );
}

export default App;
