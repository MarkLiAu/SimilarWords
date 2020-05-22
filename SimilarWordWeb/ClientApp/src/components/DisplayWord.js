import React, {Fragment, useState } from 'react';
import { Button, Panel, Label, Badge } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const DisplayWord = ({ word, idx, SearchWord }) => {
    // Declare a new state variable, which we'll call "count"
    const [count, setCount] = useState(0);
            //<p>You clicked {count} times</p>
            //<button onClick={() => setCount(count + 1)}>
            //    Click me
            //</button>

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
                    {idx === 0 ? <Label bsStyle='primary'>{word.name}</Label>  : <Link to={'/WordSearch/' + word.name} onClick={() => SearchWord(word.name)} >   {word.name} </Link>}
                    <Badge>{word.frequency}</Badge>
                    <p>{word.meaningShort}</p>
                    <Link to={{ pathname: '/wordedit/' + word.name, state: { word: word } } } >   
                            Edit
                    </Link>

                    <p className='bj_center'><ShowDictLink name={word.name}></ShowDictLink></p>
                    
                </Panel.Body>
                </Panel.Collapse>
        </Panel>

    );
}

const ShowWordName = ({ name }) => {
    //if (name == this.state.word2search) {
    //    return <b className='text-primary'>{name}</b>
    //}
    //else {
        return <Link to={'/WordSearch/' + name} >{name} </Link>
    //}
}



const ShowDictLink = ({ name }) => {
    return (
        <Fragment>
            <a title="Collins" href={'https://www.collinsdictionary.com/dictionary/english/' + name} target="_blank">Coll</a>
            <span>|</span>
            <a title='Longman' href={'https://www.ldoceonline.com/dictionary/' + name} target="_blank">Long</a>
            <span>|</span>
            <a title='Merriam Webster' href={'https://www.merriam-webster.com/dictionary/' + name} target="_blank">Merr</a>
            <span>|</span>
            <a title='Oxford Learners' href={'https://www.oxfordlearnersdictionaries.com/definition/english/' + name + '_1'} target="_blank">Oxford</a>
            <span>|</span>
            <a title='Cambridge' href={'https://dictionary.cambridge.org/dictionary/english/' + name} target="_blank">Camb</a>
            <span>|</span>
            <a title='Macmilland' href={'https://www.macmillandictionary.com/dictionary/british/' + name + '_1'} target="_blank">Macm</a>
            <span>|</span>
            <a title='Lexico' href={'https://www.lexico.com/definition/' + name} target="_blank">Lexi</a>
            <span>|</span>
            <a title='Iciba' href={'http://www.iciba.com/word?w=' + name} target="_blank">Iciba</a>
        </Fragment>
    )
}


export default DisplayWord;
