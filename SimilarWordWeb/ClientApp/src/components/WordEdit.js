import React, { Fragment, useState } from 'react';
import { Button, Panel, Label, Badge } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const EditWord = ({ word, location }) => {
    // Declare a new state variable, which we'll call "count"
    const [count, setCount] = useState(0);
    //<p>You clicked {count} times</p>
    //<button onClick={() => setCount(count + 1)}>
    //    Click me
    //</button>
    console.log(word);
    console.log(location.state.word);
    return (

        <Panel  >
            TEST
        </Panel>

    );
}


export default EditWord;
