import {
  Drawer,
  DrawerBody,
  DrawerHeader,
  DrawerOverlay,
  DrawerContent,
  DrawerCloseButton,
  useDisclosure,
  Button,
  List,
  ListIcon,
  ListItem,
  TableContainer,
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  FormControl,
  FormLabel,
  Select,
} from '@chakra-ui/react';
import { useState } from 'react';

function MoreInfo(props) {
  const { isOpen, onOpen, onClose } = useDisclosure();

  const handleClick = () => {
    onOpen();
  };
  return (
    <>
      <Button onClick={() => handleClick()} m={4}>
        View runs
      </Button>
      <Drawer onClose={onClose} isOpen={isOpen} size="xl">
        <DrawerOverlay />
        <DrawerContent>
          <DrawerCloseButton />
          <DrawerHeader>{props.item.name} - List of runs</DrawerHeader>
          <DrawerBody>
            <List spacing={3}>
              {props.item.runs.map(run => (
                <ListItem>{run.date}</ListItem>
              ))}
            </List>
          </DrawerBody>
        </DrawerContent>
      </Drawer>
    </>
  );
}

function MyList(props) {
  const [schedule, setSchedule] = useState('');
  console.log(props);
  return (
    <TableContainer>
      <Table variant="simple">
        <Thead>
          <Tr>
            <Th>Nome</Th>
            <Th>Run</Th>
            <Th>Schedule</Th>
            <Th></Th>
          </Tr>
        </Thead>
        <Tbody>
          {props?.items?.map(item => (
            <Tr>
              <Td>{item.name}</Td>
              <Td>
                <Button m={4}>Run test</Button>
              </Td>
              <Td>
                <FormControl>
                  <Select
                    placeholder=""
                    value={schedule}
                    onChange={e => setSchedule(e.target.value)}
                  >
                    <option value=""></option>
                    <option value="option1">every 30 min</option>
                    <option value="option2">every 6 hours</option>
                    <option value="option3">every day</option>
                  </Select>
                </FormControl>
              </Td>
              <Td>
                <MoreInfo item={item} />
              </Td>
            </Tr>
          ))}
        </Tbody>
      </Table>
    </TableContainer>
  );
}

export default MyList;
