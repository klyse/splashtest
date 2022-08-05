import {
  Box,
  Flex,
  Drawer,
  DrawerBody,
  DrawerFooter,
  DrawerHeader,
  DrawerOverlay,
  DrawerContent,
  DrawerCloseButton,
  useDisclosure,
  Button,
} from '@chakra-ui/react';
import { Component } from 'react';
import List from './List';

const temp = [
  {
    id: 123,
    name: 'adasdasd',
    runs: [
      {
        title: 'adasdasd',
        date: 'asdadsa',
        status: 'passed',
      },
    ],
  },
  {
    id: 123,
    name: 'adasdasd',
    runs: [
      {
        title: 'adasdasd',
        date: 'asdadsa',
        status: 'failed',
      },
    ],
  },
  {
    id: 123,
    name: 'adasdasd',
    runs: [
      {
        title: 'adasdasd',
        date: 'asdadsa',
        status: 'running',
      },
      {
        title: 'adasdasd',
        date: 'asdadsa',
        status: 'passed',
      },
      {
        title: 'adasdasd',
        date: 'asdadsa',
        status: 'failed',
      },
      {
        title: 'adasdasd',
        date: 'asdadsa',
        status: 'failed',
      },
      {
        title: 'adasdasd',
        date: 'asdadsa',
        status: 'failed',
      },
    ],
  },
];

class AllItems extends Component {
  constructor(props) {
    super(props);
    this.state = {
      items: [],
    };
  }

  componentDidMount() {
    // console.log('prova');
    fetch('http://localhost:5001/tests', {
      headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json',
      },
    })
      .then(response => response.json())
      .then(items => {
        this.setState({
          items: items,
        });
      })
      .catch(error => console.log(error));
  }
  render() {
    // console.log(temp);
    return (
      <Flex spacing={8} direction="row" gap="8">
        <Box p={5} bg="#fafafa" flexGrow="1" shadow="md" borderWidth="1px">
          {/* {this.state.items.length > 1 ? ( */}
          <List items={temp} />
          {/* ) : (
            'Add new test'
          )} */}
        </Box>
      </Flex>
    );
  }
}
export default AllItems;
