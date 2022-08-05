import React from 'react';
import { Routes, Route, Link as NavLink } from 'react-router-dom';
import Create from './Create';
import List from './List';
import {
  ChakraProvider,
  Box,
  Container,
  VStack,
  Code,
  ButtonGroup,
  Spacer,
  theme,
  Image,
  Button,
  Flex,
} from '@chakra-ui/react';
import { ColorModeSwitcher } from './ColorModeSwitcher';
import logo from '../src/splash_test.png';
import Home from './Home';

function App() {
  return (
    <ChakraProvider theme={theme}>
      <Container maxW="container.xl">
        <Box textAlign="center" fontSize="xl">
          <Flex minWidth="max-content" alignItems="center" gap="2">
            <Box p="2">
              <Image w={200} src={logo} alt="Dan Abramov" />
            </Box>
            <Spacer />
            <ButtonGroup gap="2">
              <Button m={[0, 3]} color="#3e94ff" as={NavLink} to="/create">
                Create new test
              </Button>
              <Button m={[0, 3]} color="#3e94ff" as={NavLink} to="/list">
                Check all the tests
              </Button>
            </ButtonGroup>
          </Flex>
          <Box p={5}>
            <Routes>
              <Route path="/" element={<Home />} />
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
