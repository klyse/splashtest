import {
  Drawer,
  DrawerBody,
  DrawerHeader,
  DrawerOverlay,
  DrawerContent,
  DrawerCloseButton,
  useDisclosure,
  Button,
  Accordion,
  Badge,
  AccordionItem,
  AccordionButton,
  AccordionPanel,
  AccordionIcon,
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
  Box,
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
            <Accordion>
              {props.item.runs.map(run => (
                <AccordionItem>
                  <AccordionButton>
                    <Box flex="1" textAlign="left">
                      <Badge
                        mr={4}
                        colorScheme={
                          run.status === 'passed'
                            ? 'green'
                            : run.status === 'failed'
                            ? 'red'
                            : 'yellow'
                        }
                      >
                        {run.status}
                      </Badge>
                      {run.date}
                    </Box>
                    <AccordionIcon />
                  </AccordionButton>
                  <AccordionPanel pb={4}>
                    <video
                      controls
                      src="http://localhost:5001/run/08f81165-73c2-42e2-9cd0-c2272eebcb34/video"
                    ></video>
                  </AccordionPanel>
                </AccordionItem>
              ))}
            </Accordion>
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
            <Th>Test</Th>
            <Th></Th>
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
