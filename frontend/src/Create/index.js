import {
  Badge,
  Box,
  Flex,
  Input,
  Center,
  Spacer,
  Text,
  Button,
  VStack,
  Tabs,
  TabList,
  TabPanels,
  Tab,
  TabPanel,
  FormControl,
  Stack,
  FormLabel,
} from '@chakra-ui/react';
import { useToast } from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import Empty from './Empty';

function Arrow() {
  return (
    <div style={{ margin: '20px 0' }}>
      <svg
        version="1.1"
        width="40"
        height="40"
        viewBox="-0.339 -0.606 71 213"
        enable-background="new -0.339 -0.606 71 213"
        xmlSpace="preserve"
      >
        <polygon points="44.143,0 26.487,0 26.487,167.747 0,167.747 35.314,211.89 70.631,167.747 44.143,167.747 " />
      </svg>
    </div>
  );
}

function Action({ title, type, placeholder, setItem }) {
  const [val, setVal] = useState('');
  const submit = () => {
    setItem(items => [...items, { type, value: val }]);
    setVal('');
  };
  return (
    <Flex>
      <Center w="100px">
        <Text>{title}</Text>
      </Center>
      <Spacer />
      <Center w="150px">
        <Input
          value={val}
          onChange={e => setVal(e.target.value)}
          placeholder={placeholder}
        />
      </Center>
      <Spacer />
      <Center w="100px">
        <Button onClick={submit} disabled={!val}>
          +
        </Button>
      </Center>
    </Flex>
  );
}

function FillAction({ setItem }) {
  const [val, setVal] = useState('');
  const [placeholder, setPlaceholder] = useState('');
  const submit = () => {
    setItem(items => [
      ...items,
      { type: 'fill', value: val, find: placeholder },
    ]);
    setVal('');
    setPlaceholder('');
  };
  return (
    <Flex>
      <Center w="100px">
        <Text>Fill</Text>
      </Center>
      <Spacer />
      <Center w="150px">
        <VStack>
          <Input
            value={placeholder}
            onChange={e => setPlaceholder(e.target.value)}
            placeholder="Placeholder"
          />
          <Input
            value={val}
            onChange={e => setVal(e.target.value)}
            placeholder="Value"
          />
        </VStack>
      </Center>
      <Spacer />
      <Center w="100px">
        <Button onClick={submit} disabled={!val || !placeholder}>
          +
        </Button>
      </Center>
    </Flex>
  );
}

function Step({ item }) {
  return (
    <Box
      maxW="md"
      p="8px 24px"
      borderWidth="1px"
      borderRadius="lg"
      bgColor="white"
      shadow="sm"
    >
      <Text fontSize="2xl">
        <b>{item.type}</b>
      </Text>
      <Stack direction="row">
        {item.find && <Badge colorScheme="blue">{item.find}</Badge>}
        <Badge colorScheme="green">{item.value}</Badge>
      </Stack>
    </Box>
  );
}

function Create() {
  const navigate = useNavigate();
  const toast = useToast();
  const [title, setTitle] = useState('');
  const [email, setEmail] = useState('');
  const [author, setAuthor] = useState('');
  // const [schedule, setSchedule] = useState('');
  const [items, setItem] = useState([]);
  const save = async () => {
    try {
      const response = await fetch('http://localhost:5001/tests', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Accept: 'application/json',
        },
        body: JSON.stringify({
          name: title,
          testSteps: items,
        }),
      });
      toast({
        title: 'Test created.',
        description: `We've created the test name "${title}".`,
        status: 'success',
        duration: 9000,
        isClosable: true,
      });
      navigate('/list');
    } catch (e) {
      toast({
        title: 'Error.',
        description: `Cannot create test named "${title}".`,
        status: 'error',
        duration: 9000,
        isClosable: true,
      });
    }
  };
  return (
    <Flex spacing={8} direction="row" gap="8">
      <Box p={5} shadow="md" borderWidth="1px" width={400}>
        <Tabs>
          <TabList>
            <Tab>Settings</Tab>
            <Tab>Compose</Tab>
          </TabList>

          <TabPanels>
            {' '}
            <TabPanel>
              <VStack spacing={4}>
                <FormControl>
                  <FormLabel>Test name</FormLabel>
                  <Input
                    value={title}
                    onChange={e => setTitle(e.target.value)}
                    placeholder="Name"
                  />
                </FormControl>
                <FormControl>
                  <FormLabel>Author</FormLabel>
                  <Input
                    placeholder="Author"
                    value={author}
                    onChange={e => setAuthor(e.target.value)}
                  />
                </FormControl>

                <FormControl>
                  <FormLabel>Send alert to</FormLabel>
                  <Input
                    placeholder="Email"
                    value={email}
                    onChange={e => setEmail(e.target.value)}
                  />
                </FormControl>
              </VStack>
            </TabPanel>
            <TabPanel>
              <VStack spacing={4}>
                <Action
                  type="visit"
                  title="Visit"
                  placeholder="Url"
                  setItem={setItem}
                />
                <Action
                  type="click"
                  title="Click"
                  placeholder="Label"
                  setItem={setItem}
                />
                <Action
                  type="contains"
                  title="Contains"
                  placeholder="Text"
                  setItem={setItem}
                />
                <FillAction setItem={setItem} />
              </VStack>
            </TabPanel>
          </TabPanels>
        </Tabs>
        <Button
          colorScheme="blue"
          disabled={items.length === 0}
          onClick={save}
          w="100%"
        >
          Save
        </Button>
      </Box>
      <Box p={5} bg="#fafafa" flexGrow="1" shadow="md" borderWidth="1px">
        {items.length === 0 && (
          <Center flexDirection="column">
            <Text>
              Just a few clicks to create, customize, run and save your test.
            </Text>
            <Box maxWidth={400}>
              <Empty />
            </Box>
          </Center>
        )}
        <VStack divider={<Arrow />} spacing={4} align="center">
          {items.map(item => (
            <Step item={item} />
          ))}
        </VStack>
      </Box>
    </Flex>
  );
}

export default Create;
