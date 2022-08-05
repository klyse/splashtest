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
  StackDivider,
} from '@chakra-ui/react';
import { useState } from 'react';

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
        <Button onClick={submit}>+</Button>
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
      <Badge colorScheme="green">{item.value}</Badge>
    </Box>
  );
}

function Create() {
  const [items, setItem] = useState([]);
  return (
    <Flex spacing={8} direction="row" gap="8">
      <Box p={5} shadow="md" borderWidth="1px">
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
        </VStack>
      </Box>
      <Box p={5} bg="#fafafa" flexGrow="1" shadow="md" borderWidth="1px">
        {items.length === 0 && <Text>Create your test</Text>}
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
