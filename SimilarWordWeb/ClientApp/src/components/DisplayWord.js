import React, {Fragment, useState } from 'react';
import { Button, Panel, Label, Badge } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import WordInfoDisplay from './WordInfoDisplay';
import { GetTokenHeader } from './CommTools';

const DisplayWord = ({ word, idx, SearchWord }) => {
    // Declare a new state variable, which we'll call "count"
    const [result, setResult] = useState('');
            //<p>You clicked {count} times</p>
            //<button onClick={() => setCount(count + 1)}>
            //    Click me
            //</button>


    const CallUpdateApi = (e) => {
        //alert("in CallUpdateApi");
        console.log("CallUpdateApi");
        e.preventDefault();
        word.viewInterval = -1;
        fetch('api/Memory/' + word.name, {
            method: 'PUT',
            body: JSON.stringify(word),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': GetTokenHeader()
            }
        })
            .then(response => { console.log(response); })
    }



    return (

        <Panel className='bj_margin_b' eventKey={idx.toString()} id={'panel' + word.name }  >
            <Panel.Heading >
                <Panel.Title toggle >
                    {word.name}
                    {' ' + word.pronounciation+'   '+ (word.frequency>10000? '*' :'')}
            </Panel.Title>
            </Panel.Heading>
            <Panel.Collapse>

            <Panel.Body>
                    <WordInfoDisplay word={word}></WordInfoDisplay>
                    <button hidden={word.viewInterval>=-1} onClick={CallUpdateApi}>Start Memory</button>
                </Panel.Body>
                </Panel.Collapse>
        </Panel>

    );
}

export default DisplayWord;
